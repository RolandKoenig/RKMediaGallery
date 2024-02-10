# Cleanup previous contents of publish directory
Remove-Item ../publish -Recurse -Force
mkdir ../publish

# Create macOS app package
dotnet publish -c Release -r osx-arm64 --self-contained -o ../publish/RKMediaGallery/Contents/MacOS -p:PublishReadyToRun=true ../src/RKMediaGallery/RKMediaGallery.csproj
cp -r ../macos-app-template/* ../publish/RKMediaGallery/Contents
mv ../publish/RKMediaGallery ../publish/RKMediaGallery.app