# dotnet-opencover
A simple wrapper nuget package to call opencover using the dotnet cli "dotnet opencover"

_This does not bundle in OpenCover itself. You need to include that as a dependency._

## Usage
`dotnet opencover <opencover args>`

or

`dotnet opencover --opencover-version x.x.x <opencover args>`

_This does not download the specific verison. Only targets the version which would be downloaded by nuget. You will need to add a reference to the version and `dotnet restore` to get it._

Currently assumes OpenCover is located in `%userprofile%/.nuget/packages/opencover/<versionspec>/tools/OpenCover.Console.exe`
