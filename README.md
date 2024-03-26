# Geo-Feature-Finder

## Table of Contents

- [Description](#description)

- [Usage](#usage)

## Description

This is a simple desktop application with a basic GUI that allows users to quickly find the shortest distance and closest latitude and longitude to various geographical features based on a given latitude and longitude. **This application is intended to work primarily with Habitats and Species data from Defra's MAGIC**.

Upon running the application, the user is presented with a simple GUI that allows them to load an input JSON file containing their dataset, input a latitude and longitude and specify an output file path. The user can the click the **Run** button to find the **shortest distance** and **closest latitude and longitude** to the geographical features in the dataset based on the given latitude and longitude. The results are then saved to a CSV file at the specified output file path.

## Usage

To run this application locally, first clone the reposity and open the project in your IDE. Then, run the following command in the terminal to install the required dependencies:

```sh
dotnet restore
```

You can then run the application by running the following command in the terminal:

```sh
dotnet run
```

If you wish to build the application as a standalone executable, you can run the following command in the terminal (replacing `<output-directory>` with the desired output directory):

```sh
dotnet publish -c Release -r win-x64 --self-contained -o <output-directory>
```

This will create a standalone executable in the specified output directory that can be run on any Windows machine.

Since this application uses WinForms, it is only designed to be compatible with Windows machines.
