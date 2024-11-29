# ReferenceManager

A simple C# application for managing BibTeX references.


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

  
