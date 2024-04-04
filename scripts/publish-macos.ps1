# Cleanup previous contents of publish directory
Remove-Item ../publish -Recurse -Force
mkdir ../publish

# Create macOS app package (RK Media Gallery)
dotnet publish -c Release -r osx-arm64 --self-contained -o ../publish/RKMediaGallery/Contents/MacOS -p:PublishReadyToRun=true ../src/RKMediaGallery/RKMediaGallery.csproj
cp -r ../macos-app-template/mediagallery/* ../publish/RKMediaGallery/Contents
mv ../publish/RKMediaGallery ../publish/RKMediaGallery.app

# Create macOS app package (RK Media Gallery)
dotnet publish -c Release -r osx-arm64 --self-contained -o ../publish/RKMediaGalleryConvertUtility/Contents/MacOS -p:PublishReadyToRun=true ../src/RKMediaGallery.PictureConvertUtility/RKMediaGallery.PictureConvertUtility.csproj
cp -r ../macos-app-template/mediagallery-convertutility/* ../publish/RKMediaGalleryConvertUtility/Contents
mv ../publish/RKMediaGalleryConvertUtility ../publish/RKMediaGalleryConvertUtility.app