![Coverage](https://img.shields.io/badge/coverage-80.34%25-green)

[![CI for xUnit Tests with Versioning](https://github.com/ohjelmistotuotanto-miniprojekti/ohjelmistotuotanto-miniprojekti/actions/workflows/ci.yml/badge.svg)](https://github.com/ohjelmistotuotanto-miniprojekti/ohjelmistotuotanto-miniprojekti/actions/workflows/ci.yml)

# ReferenceManager

A simple C# application for managing BibTeX references.

# Project backlog

https://docs.google.com/spreadsheets/d/1109rdzZxVDn1R0a3zG7uqhMdJKA6VQSDgxw8KljecDI

# Report
https://docs.google.com/document/d/1q0EXRurnsQLYOGQiM3NbFrLV6FZjRvavblajhf58gcQ/edit?usp=sharing

# Definition of done

### Sprint 1:

1. CI/CD environment for automated unit testing
2. Ability to add journal articles
   1. System must allow users to add journal articles as references
   2. System asks confirmation about operation
   3. System gives a infomessage about success /failure
3. Generate BibTex file for LaTeX documents
   1. The system must provide functionality to generate a BibTeX-formatted file from the references for use in LaTeX documents.
   2. System will print the BibTeX -formatted input
   3. System gives a infomessage about success /failure


### Sprint 2:
1. Ability to add inproceedings articles
   1. A CLI command is available for adding "inproceedings" articles, allowing the user to input required details (e.g., title, authors, year, book title, and publisher)  through interactive prompts.
   2. Validation is in place to ensure required fields are filled.
   3. System asks confirmation about operation
   4. System gives a infomessage about success /failure
   5. Successfully added articles are stored in the system and can be viewed with a CLI command.
2. Add optional fields into articles
   1. User can add optional fields into articles or inproceedings.
   2. Validation ensures all required fields for the selected type are provided.
   3. System gives a infomessage about success /failure.
   4. Successfully added articles are stored in the system and can be viewed with a CLI command.
3. Program has proper validation for input fields, won't crash if wrong input format
   1. If a wrong datatype of input is given, the program will give an error message instead of crashing.
   2. If wrong input is given, program will ask for a valid input.
4. Support for multiple authors
   1. The CLI command for adding an article allows specifying multiple authors through repeated prompts for each author.
   2. Validation ensures at least one author is provided.
   3. Articles with multiple authors are stored correctly and appear properly formatted in the human-readable reference list and BibTeX output.

### Sprint 3:
1. Support for multiple authors
   1. The CLI command for adding an article allows specifying multiple authors through repeated prompts for each author.
   2. Validation ensures at least one author is provided.
   3. Articles with multiple authors are stored correctly and appear properly formatted in the human-readable reference list and BibTeX output.
2. Generate human-readable reference list
   1. The system must allow users to generate a human-readable list of references stored in the system
3. References can be filtered by author, year, title or publication.
   1. System allows user to filter references  for example by year, author, title or publication.
   2. System will print the filtered references.



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
6. Code coverage:
   ```
   dotnet test --collect:"XPlat Code Coverage"
   ```
7. Linting:
   ```
   dotnet format ReferenceManager.sln --verify-no-changes # no formatting just checking
   dotnet format ReferenceManager.sln # with automatic formatting
   ```
