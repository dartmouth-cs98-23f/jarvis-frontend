#!/bin/bash

# This script copies the SignalR DLLs from the ClientLibrary project to the <PathToUnityProject>/Assets/Plugins/SignalR

# PathToClientLibrary : Absolute path to the ClientLibrary project, provided as a command line argument
# PathToUnityProject  : The path to the Unity project, provided as a command line argument

# ensure two command line arguments are provided
if [ $# -ne 2 ]; then
    echo "Usage: $0 <PathToClientLibrary> <PathToUnityProject>"
    exit 1
fi

PathToClientLibrary=$1
PathToUnityProject=$2

# check if ClientLibrary project directory exists
if [ ! -d "$PathToClientLibrary"  ]; then
    echo "Error: ClientLibrary project directory does not exist: '$PathToClientLibrary'. Specify existing ClientLibrary project."
    exit 1
fi

# check if Unity project directory exists
if [ ! -d "$PathToUnityProject"  ]; then
    echo "Unity project directory '$PathToUnityProject' does not exist."
    echo "Creating project directory: '$PathToUnityProject' ..."
    mkdir -p $PathToUnityProject
fi


# define path to store the SignalR DLLs in the Unity project
# we store the DLLs in the Assets/Plugins/SignalR directory
PathToUnityProjectDLLs=$(realpath $PathToUnityProject)/Assets/Plugins/SignalR

# switch directory to the ClientLibrary project directory
cd $PathToClientLibrary

# publish the ClientLibrary project to generate the DLLs
echo "Publishing ClientLibrary project '$PathToClientLibrary' to generate DLLs..."
dotnet publish -c Release

# navigate to the publish directory
# for Unity projects, the target framework is netstandard2.1
TargetFramework="netstandard2.1"
cd bin/Release/$TargetFramework/publish

# copy the SignalR DLLs to the Unity project
echo "Copying SignalR DLLs to Unity project..."

mkdir -p $PathToUnityProjectDLLs
cp -r *.dll $PathToUnityProjectDLLs

echo "Copied SignalR DLLs to path: '$PathToUnityProjectDLLs'"