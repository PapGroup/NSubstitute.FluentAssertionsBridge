using System;
using System.Linq;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    public static int Main () => Execute<Build>(x => x.RunTests, x=> x.Publish);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;
    [Parameter("Directory which artifacts of build will be placed - Default is 'artifacts' folder in root directory of project")]
    AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    [Solution] readonly Solution Solution;
    [GitVersion] readonly GitVersion GitVersion;

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            DotNetClean(a => a.SetProject(Solution));
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(a => a.SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(a =>
                a.SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    .SetNoRestore(true)
                    .SetAssemblyVersion(GitVersion.AssemblySemVer)
                    .SetFileVersion(GitVersion.AssemblySemFileVer)
                    .SetInformationalVersion(GitVersion.InformationalVersion));
        });

    Target RunTests => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(a =>
                a.SetProjectFile(Solution)
                    .SetConfiguration(Configuration)
                    .SetNoRestore(true)
                    .SetNoBuild(true));
        });

    Target Pack => _ => _
        .DependsOn(RunTests)
        .Executes(() =>
        {
            DotNetPack(s => s
                .SetProject(Solution.GetProject("PAP.NSubstitute.FluentAssertionsBridge"))
                .SetOutputDirectory(ArtifactsDirectory)
                .SetNoBuild(true)
                .SetNoRestore(true)
                .SetConfiguration(Configuration)
                .SetVersion(GitVersion.NuGetVersionV2));
        });

    Target Publish => _ => _
        .DependsOn(Pack)
        .Executes(() =>
        {
            //TODO: Publish nuget packages to nuget.org
        });
}
