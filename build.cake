#load "nuget:?package=Afc.CommonBuild.Cake.Common"

// Change "Release" to be the name of an alternative Task
// if you want to customize your build further.
// target = Argument("target", "Release");

// Set solutionFile if you have multiple .sln files.
// solutionFile = null;

// Visual Studio version is determined **automatically** from your
// solution file. But, you can force it to use a specific one.
// "14.0" = Visual Studio 2015
// "15.0" = Visual Studio 2017
visualStudioVersion = "15.0";

// configuration = "Release";

// This is the filter use for code coverage.
// Visit https://github.com/OpenCover/opencover/wiki/Usage for details on how to specify the filter.
// coverageFilter = "+[*]* -[*Test*]*";

// Specify any csproj files that you want packaged and published in addition to those built via Octopack
// deploymentNuspecs are for those that will be deployed via Octopus. Typically static websites or Rules.
// libraryNuspecs are for those that will goto the nuget repository for use in other projects.
// Delimit multiple csprojs with a comma. Use forward slashes '/' so you don't have to escape them.
// You can specify a nuspec file if you don't have a csproj that goes with it.
// The paths are relative to the root of the solution.
// deploymentNuspecs = "Path/To/Your.csproj";
// libraryNuspecs = null;

// ============================================================================
// NPM Build needs to run before Build, 
// otherwise Visual Studio will fail when trying to compile TypeScript.
// ============================================================================


// For more information on the Cake build tool goto http://cakebuild.net/
// See https://apps.afcorp.afg/bootcamp/View/185 on how to further customize your build.
RunTarget(target);
