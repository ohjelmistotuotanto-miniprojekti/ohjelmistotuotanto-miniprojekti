import os
import xml.etree.ElementTree as ET

def find_most_recent_coverage_file(base_path):
    """
    Finds the most recent coverage.cobertura.xml file in dynamically generated TestResults folders.
    """
    latest_file = None
    latest_time = 0

    for root, dirs, files in os.walk(base_path):
        for file in files:
            if file == "coverage.cobertura.xml":
                file_path = os.path.join(root, file)
                file_time = os.path.getmtime(file_path)
                if file_time > latest_time:
                    latest_time = file_time
                    latest_file = file_path

    return latest_file

def parse_coverage(file_path):
    """
    Parses the coverage.cobertura.xml file and extracts the code coverage percentage.
    """
    tree = ET.parse(file_path)
    root = tree.getroot()
    # Extract the "line-rate" attribute (Cobertura format)
    coverage = float(root.attrib["line-rate"]) * 100
    return coverage

def get_badge_color(coverage):
    """
    Returns a color based on the coverage percentage.
    """
    if coverage >= 90:
        return "brightgreen"
    elif coverage >= 80:
        return "green"
    elif coverage >= 70:
        return "yellow"
    elif coverage >= 50:
        return "orange"
    else:
        return "red"

# Define the base directory where TestResults is located
base_dir = "tests/ReferenceManager.Tests/TestResults"

# Find the most recent coverage file dynamically
coverage_file = find_most_recent_coverage_file(base_dir)

if not coverage_file:
    print(f"Coverage file not found in {base_dir}")
    exit(1)

print(f"Found coverage file at: {coverage_file}")

# Parse the coverage and generate the badge
coverage = parse_coverage(coverage_file)
print(f"Code coverage: {coverage:.2f}%")

# Determine badge color based on coverage
badge_color = get_badge_color(coverage)

# Save badge URL (Shields.io example)
badge_url = f"https://img.shields.io/badge/coverage-{coverage:.2f}%25-{badge_color}"
print(f"Badge URL: {badge_url}")

# Write badge to Markdown
with open("coverage_badge.md", "w") as badge_file:
    badge_file.write(f"![Coverage]({badge_url})\n")
