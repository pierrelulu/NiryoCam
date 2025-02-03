% Définir le dossier contenant les images
inputFolder = uigetdir(pwd, 'Sélectionnez le dossier contenant les images');
if inputFolder == 0
    error('Aucun dossier sélectionné.');
end

% Définir le dossier de sortie pour les images converties
outputFolder = fullfile(inputFolder, 'Converted_BMP');
if ~exist(outputFolder, 'dir')
    mkdir(outputFolder);
end

% Lire toutes les images du dossier
imageFiles = dir(fullfile(inputFolder, '*.*'));
validExtensions = {'.jpg', '.jpeg', '.png', '.tif', '.tiff', '.gif'};

for i = 1:length(imageFiles)
    [~, ~, ext] = fileparts(imageFiles(i).name);
    
    % Vérifier si l'extension est valide
    if any(strcmpi(ext, validExtensions))
        % Charger l'image
        img = imread(fullfile(inputFolder, imageFiles(i).name));
        
        % Construire le nom du fichier de sortie
        [~, name, ~] = fileparts(imageFiles(i).name);
        outputFile = fullfile(outputFolder, strcat(name, '.bmp'));
        
        % Sauvegarder l'image en BMP
        imwrite(img, outputFile, 'bmp');
        fprintf('Image convertie : %s -> %s\n', imageFiles(i).name, outputFile);
    end
end

fprintf('Conversion terminée. Toutes les images valides ont été enregistrées en BMP dans %s\n', outputFolder);
