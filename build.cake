var target = Argument("Target", "Default");
var configuration =
    HasArgument("Configuration") ? Argument<string>("Configuration") :
    EnvironmentVariable("Configuration", "Release");

var artefactsDirectory = Directory("./Artefacts");

Task("Clean")
    .Description("Cleans the artefacts, bin and obj directories.")
    .Does(() =>
    {
        CleanDirectory(artefactsDirectory);
        DeleteDirectories(GetDirectories("**/bin"), new DeleteDirectorySettings() { Force = true, Recursive = true });
        DeleteDirectories(GetDirectories("**/obj"), new DeleteDirectorySettings() { Force = true, Recursive = true });
    });

Task("Restore")
    .Description("Restores NuGet packages.")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetRestore();
    });

Task("Build")
    .Description("Builds the solution.")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        DotNetBuild(
            ".",
            new DotNetBuildSettings()
            {
                Configuration = configuration,
                NoRestore = true,
            });
    });

Task("Test")
    .Description("Runs unit tests and outputs test results to the artefacts directory.")
    .DoesForEach(
        GetFiles("./Tests/**/*.csproj"),
        project =>
        {
            DotNetTest(
                project.ToString(),
                new DotNetTestSettings()
                {
                    Configuration = configuration,
                    NoBuild = true,
                    NoRestore = true,
                    ArgumentCustomization = args => 
                    {
                        var newArgs = args
                            .Append("--results-directory").Append(artefactsDirectory.Path.ToString())
                            .Append("--report-xunit-trx")
                            .Append("--report-xunit-trx-filename")
                            .Append($"{project.GetFilenameWithoutExtension()}.trx")
                            .Append("--coverage")
                            .Append("--coverage-output-format")
                            .Append("cobertura")
                            .Append("--coverage-output")
                            .Append("coverage.cobertura.xml");

                        if (!BuildSystem.IsLocalBuild)
                        {
                            newArgs = newArgs.Append("--filter").Append("Category!=Integration");
                        }
                        return newArgs;
                    }
                });
        });

Task("Pack")
    .Description("Creates NuGet packages and outputs them to the artefacts directory.")
    .Does(() =>
    {
        DotNetPack(
            ".",
            new DotNetPackSettings()
            {
                Configuration = configuration,
                IncludeSymbols = true,
                MSBuildSettings = new DotNetMSBuildSettings()
                {
                    ContinuousIntegrationBuild = !BuildSystem.IsLocalBuild,
                },
                NoBuild = true,
                NoRestore = true,
                OutputDirectory = artefactsDirectory,
            });
    });

Task("Default")
    .Description("Cleans, restores NuGet packages, builds the solution, runs unit tests and then creates NuGet packages.")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("Pack");

RunTarget(target);
