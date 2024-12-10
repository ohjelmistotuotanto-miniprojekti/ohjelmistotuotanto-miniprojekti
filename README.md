[![CI for xUnit Tests with Versioning](https://github.com/ohjelmistotuotanto-miniprojekti/ohjelmistotuotanto-miniprojekti/actions/workflows/ci.yml/badge.svg)](https://github.com/ohjelmistotuotanto-miniprojekti/ohjelmistotuotanto-miniprojekti/actions/workflows/ci.yml)

# ReferenceManager

A simple C# application for managing BibTeX references.

# Project backlog
https://docs.google.com/spreadsheets/d/1109rdzZxVDn1R0a3zG7uqhMdJKA6VQSDgxw8KljecDI

# Definition of done
Sprint 1:
1. CI/CD environment for automated unit testing 
2. Ability to add journal articles
    1. System must allow users to add journal articles as references
    2. System asks confirmation about operation
    3. System gives a infomessage about success /failure
3. Generate BibTex file for LaTeX documents
    1. The system must provide functionality to generate a BibTeX-formatted file from the references for use in LaTeX documents.
    2. System will print the BibTeX -formatted input
    3. System gives a infomessage about success /failure

## Developer / Collaborator instructions / Getting started

Follow these steps to set up the project on your local machine.

### Prerequisites

Ensure you have the following installed:

- **.NET SDK** (9.0 or later) – [Download here](https://dotnet.microsoft.com/en-us/download)
- **Git** – [Download here](https://git-scm.com/)
- **Code Editor**:
  - Visual Studio (recommended) – [Download here](https://visualstudio.microsoft.com/)
  - OR Visual Studio Code with the [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)

---

### Installation

### Installation and Setup

1. Clone the repository to your local machine:
   ```
   git clone <[repository-url](https://github.com/ohjelmistotuotanto-miniprojekti/ohjelmistotuotanto-miniprojekti)>
   cd ReferenceManager
   ```
2. Restore all required NuGet packages:
    ```
    dotnet restore
    ```
3. Build the solution to verify everything works:
    ```
    dotnet build --configuration Release
    ```
4. Run the application to test the main functionality:
    ```
    dotnet run --project ReferenceManager/ReferenceManager.csproj
    ```
5. Run tests to ensure everything works:
    ```
    dotnet test --configuration Release
    ```
6. Linting:
    ```
    dotnet format ReferenceManager.sln --verify-no-changes # no formatting just checking
    dotnet format ReferenceManager.sln # with automatic formatting
    ```


  
