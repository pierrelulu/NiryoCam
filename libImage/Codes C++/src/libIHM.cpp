#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <windows.h>
#include <cmath>
#include <vector>
#include <ctime>
#include <stack>
#include <thread>


#include "../include/libIHM.h"




ClibIHM::ClibIHM() {

	this->nbDataImg = 0;
	this->dataFromImg.clear();
	this->imgPt = NULL;
}

ClibIHM::ClibIHM(int nbChamps, byte* data, int stride, int nbLig, int nbCol){
	this->nbDataImg = nbChamps;
	this->dataFromImg.resize(nbChamps);

	this->imgPt = new CImageCouleur(nbLig, nbCol);
	//CImageCouleur refC(nbLig, nbCol);
	CImageCouleur out(nbLig, nbCol);

	// RECUPERTION DES DONNEES DE L'IMAGE
	byte* pixPtr = (byte*)data;
	
	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			this->imgPt->operator()(y, x)[0] = pixPtr[3 * x + 2];
			//refC(y, x)[0] = refPtr[3 * x + 2];
			this->imgPt->operator()(y, x)[1] = pixPtr[3 * x + 1];
			//refC(y, x)[1] = refPtr[3 * x + 1];
			this->imgPt->operator()(y, x)[2] = pixPtr[3 * x ];
			//refC(y, x)[2] = refPtr[3 * x];
		}
		pixPtr += stride; // largeur une seule ligne gestion multiple 32 bits
		//refPtr += stride;
	}

	//out = this->imgPt->plan().morphologie("median", elemStruct::croix(5));
	//this->imgPt->plan().morphologie("median", elemStruct::croix(5)).sauvegarde("out_test");

	// OVERWRITE OLD IMG : GROS SKIP SA MERE

	std::string perspective_folder = "C:\\Users\\luttm\\Documents\\Devoirs\\FISA3\\Application Csharp\\NiryoCam\\images_persp\\";
	std::string calibration_folder = "C:\\Users\\luttm\\Documents\\Devoirs\\FISA3\\Application Csharp\\NiryoCam\\images_calib\\";
	std::string object_folder = "C:\\Users\\luttm\\Documents\\Devoirs\\FISA3\\Application Csharp\\NiryoCam\\images_obj\\";
	
	for (int k = 0; k < 4; k++)
	{
		std::string img_persp_file_name = perspective_folder + std::to_string(k+1) + ".bmp";
        std::string img_calib_file_name = calibration_folder + std::to_string(k+1) + ".bmp";
        std::string img_objet_file_name = object_folder + std::to_string(k + 1) + ".bmp";

		CImageCouleur imageDepthColor(img_persp_file_name);
		CImageNdg imageDepth = imageDepthColor.plan(2);
		imageDepth.sauvegarde("image_base");
		CImageNdg labelsDepth, squeletteDepth;
		int numDepth = 0;
		process_image(imageDepth, labelsDepth, numDepth, squeletteDepth, 0.7f);
		squeletteDepth.sauvegarde("image_squelette");

        // 4) Correction des lignes => correctedLabelsDepth
        std::vector<std::vector<std::pair<int, int>>> correctionsDepth;
        CImageNdg correctedLabelsDepth;
        correctLines(labelsDepth, numDepth, correctionsDepth, correctedLabelsDepth);

        // 5) Champ de correction => corrFieldDepth
        std::vector<std::vector<float>> corrFieldDepth;
        buildCorrectionField(labelsDepth, numDepth, correctionsDepth, corrFieldDepth);

        // Interpolation verticale
        verticalInterpolation(corrFieldDepth);

        // Flou gaussien => corrFieldDepthBlur
        std::vector<std::vector<float>> corrFieldDepthBlur;
        gaussianBlur2D(corrFieldDepth, 20.0f, corrFieldDepthBlur);

        // 6) Calcul de scale_y (mm/pixel) depuis "correctedLabelsDepth"
        //    On convertit correctedLabelsDepth en matrice d'entiers :
        int hDepth = correctedLabelsDepth.lireHauteur();
        int wDepth = correctedLabelsDepth.lireLargeur();
        std::vector<std::vector<int>> correctedLabelsInt(hDepth, std::vector<int>(wDepth, 0));

        for (int i = 0; i < hDepth; i++)
        {
            for (int j = 0; j < wDepth; j++)
            {
                correctedLabelsInt[i][j] = (int)correctedLabelsDepth(i, j);
            }
        }

        float scaleY = 1.0f; // computeScaleY(correctedLabelsInt, numDepth);
        std::vector<int> linePositions = computeScaleY(correctedLabelsInt, numDepth);
        //TEMPOROAIRE TODO
        float scaleX = scaleY;
        std::cout << "[Info] scaleY (mm/pixel) = " << scaleY << std::endl;

        // =============== PARTIE 2 : IMAGE VOLUME ===============
        // 7) Charger la deuxième image (volume)
        CImageCouleur imageVolumeColor(img_calib_file_name);
        CImageNdg imageVolume = imageVolumeColor.plan(2);

        // Squelettisation et labélisation
        CImageNdg labelsVolume, squeletteVolume;
        int numVolume = 0;
        process_image(imageVolume, labelsVolume, numVolume, squeletteVolume, 0.7f);
        squeletteVolume.sauvegarde("02_squeletteVolume");

        // 8) Appliquer le champ corrFieldDepthBlur sur la 2e image => correctedVolume
        CImageNdg correctedVolume;
        applyCorrectionField(labelsVolume, corrFieldDepthBlur, correctedVolume);
        correctedVolume.sauvegarde("03_correctedVolume");

        // === NOUVEAU : Re-labellisation / Correction #2 comme en MATLAB
        // 9) correctLines(...) sur correctedVolume => correctedLabels2
        std::vector<std::vector<std::pair<int, int>>> corrections2;
        CImageNdg correctedLabels2;
        correctLines(correctedVolume, numVolume, corrections2, correctedLabels2);
        correctedLabels2.sauvegarde("04_corrected_labels2");

        // 10) buildCorrectionField(...) => depthMap2
        std::vector<std::vector<float>> depthMap2;
        buildCorrectionField(correctedVolume, numVolume, corrections2, depthMap2);
        // On pourrait l'enregistrer via mat2gray si on veut (ex: recompose en NDG)

        //// 11) Calcul de scale_z (mm/pixel) en se basant sur correctedVolume et depthMap2
        ////     On convertit correctedVolume en std::vector<std::vector<int>>
        //int hVol = correctedVolume.lireHauteur();
        //int wVol = correctedVolume.lireLargeur();
        //std::vector<std::vector<int>> newLabelsInt(hVol, std::vector<int>(wVol, 0));
        //for (int i = 0; i < hVol; i++)
        //{
        //    for (int j = 0; j < wVol; j++)
        //    {
        //        newLabelsInt[i][j] = (int)correctedVolume(i, j);
        //    }
        //}

        //float scaleZ = computeScaleZ(newLabelsInt, depthMap2, 50.0f);
        //std::cout << "[Info] scaleZ (mm/pixel) = " << scaleZ << std::endl;

        // =============== PARTIE 3 : GÉNÉRATION DU NUAGE 3D FINAL ===============
        // 12) Construire le nuage (X2, Y2, Z2) depuis correctedLabels2 + depthMap2
        //     On applique la même transformation x=-col, y=row - z, z=-z  (param "true")
        std::vector<float> X2, Y2, Z2;
        build3DPointsFromCorrection(correctedVolume, depthMap2, X2, Y2, Z2, true);

        // 13) Mise à l'echelle des coordonnées (X2, Y2, Z2) en mm
        std::vector<float> X_sort, Y_sort, Y_interpol, Z_sort;
        sortByColumn(X2, Y2, Z2, X_sort, Y_sort, Z_sort, 1);

        std::vector<bool> valid_index;
        interp1_previous(linePositions, Y_sort, Y_interpol, valid_index);

        std::vector<float> X_norm, Y_norm, Z_norm;
        extract_valid(X_sort, Y_interpol, Z_sort, valid_index, X_norm, Y_norm, Z_norm);

        std::vector<float> calibre[3], gradient[3];
        generate_calibre(linePositions, X_norm, Y_norm, Z_norm, calibre, gradient);

        std::string output_folder = "C:\\Users\\luttm\\Documents\\Devoirs\\FISA3\\Application Csharp\\NiryoCam\\output\\";

        exportPointsToPLY(calibre[0], calibre[1], calibre[2], output_folder + "my_cloud_calibre.ply");
        exportPointsToPLY(gradient[0], gradient[1], gradient[2], output_folder + "my_cloud_gradient.ply");

        std::pair<float, float> calibration_data[3];
        std::vector<float> X_calib, Y_calib, Z_calib;
        computeCalibration(calibre, calibration_data, X_norm, Y_norm, Z_norm);
        applyCalibration(calibration_data, X_norm, Y_norm, Z_norm, X_calib, Y_calib, Z_calib);

        // 14) Sauvegarder le nuage en .ply
        exportPointsToPLY(X_calib, Y_calib, Z_calib, output_folder + "my_cloud_final_" + std::to_string(k) + ".ply");

        // RECONSTRUCTION 3D OBJET ---------------------------
        // 
        // 7) Charger la deuxième image (volume)
        CImageCouleur imageObjetColor(img_objet_file_name);
        CImageNdg imageObjet = imageObjetColor.plan(2);

        // Squelettisation et labélisation
        CImageNdg labelsObjet, squeletteObjet;
        int numObjet = 0;
        process_image(imageObjet, labelsObjet, numObjet, squeletteObjet, 0.85f);
        squeletteObjet.sauvegarde("02_squeletteObjet");

        // 8) Appliquer le champ corrFieldDepthBlur sur la 2e image => correctedVolume
        CImageNdg correctedObjet = labelsObjet;
        //applyCorrectionField(labelsObjet, corrFieldDepthBlur, correctedObjet);
        //correctedObjet.sauvegarde("03_correctedObjet");

        // === NOUVEAU : Re-labellisation / Correction #2 comme en MATLAB
        // 9) correctLines(...) sur correctedVolume => correctedLabels2
        std::vector<std::vector<std::pair<int, int>>> corrections3;
        CImageNdg correctedLabels3;
        correctLines(correctedObjet, numObjet, corrections3, correctedLabels3);
        correctedLabels3.sauvegarde("04_corrected_labels3");

        // 10) buildCorrectionField(...) => depthMap2
        std::vector<std::vector<float>> depthMap3;
        buildCorrectionField(correctedObjet, numObjet, corrections3, depthMap3);
        // On pourrait l'enregistrer via mat2gray si on veut (ex: recompose en NDG)

        //// 11) Calcul de scale_z (mm/pixel) en se basant sur correctedVolume et depthMap2
        ////     On convertit correctedVolume en std::vector<std::vector<int>>
        //int hVol = correctedVolume.lireHauteur();
        //int wVol = correctedVolume.lireLargeur();
        //std::vector<std::vector<int>> newLabelsInt(hVol, std::vector<int>(wVol, 0));
        //for (int i = 0; i < hVol; i++)
        //{
        //    for (int j = 0; j < wVol; j++)
        //    {
        //        newLabelsInt[i][j] = (int)correctedVolume(i, j);
        //    }
        //}

        //float scaleZ = computeScaleZ(newLabelsInt, depthMap2, 50.0f);
        //std::cout << "[Info] scaleZ (mm/pixel) = " << scaleZ << std::endl;

        // =============== PARTIE 3 : GÉNÉRATION DU NUAGE 3D FINAL ===============
        // 12) Construire le nuage (X2, Y2, Z2) depuis correctedLabels2 + depthMap2
        //     On applique la même transformation x=-col, y=row - z, z=-z  (param "true")
        std::vector<float> X3, Y3, Z3;
        build3DPointsFromCorrection(correctedObjet, depthMap3, X3, Y3, Z3, true);
        exportPointsToPLY(X3, Y3, Z3, output_folder + "my_cloud_final_object_" + std::to_string(k) + ".ply");
	}

	

	
	// RECONTRUCTION DU RETOUR IMAGE
	pixPtr = (byte*)data;
	
	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			pixPtr[3 * x + 2] = out(y, x)[0];
			pixPtr[3 * x + 1] = out(y, x)[1];
			pixPtr[3 * x] = out(y, x)[2];
		}
		pixPtr += stride; // largeur une seule ligne gestion multiple 32 bits
	}
	
}


ClibIHM::~ClibIHM() {
	
	if (imgPt)
		(*this->imgPt).~CImageCouleur(); 
	this->dataFromImg.clear();
}