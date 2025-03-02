# Common cleanup
dotnet clean "../RKMediaGallery.core.slnf"

# Build and test
dotnet build -c Debug "../RKMediaGallery.core.slnf"
dotnet test -c Debug "../RKMediaGallery.core.slnf"