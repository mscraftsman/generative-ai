#!/bin/bash

# --- Check for Bash ---
if [[ "$BASH_VERSION" == "" ]]; then
  echo "Error: This script must be run with Bash."
  echo "Please use: bash ./setup.sh"
  exit 1
fi

# --- Configuration ---
VENV_DIR=".venv"  # Name of the virtual environment directory
SCRIPT_DIR=$(dirname "$0") # Directory of the script
PROJECT_DIR=$(cd "$SCRIPT_DIR" && pwd) # Get the absolute path of the script's directory

# --- Functions ---
function create_venv() {
  echo "Creating virtual environment in ${VENV_DIR}..."
  python3 -m venv "${VENV_DIR}"
  if [[ $? -ne 0 ]]; then
      echo "Error: Could not create virtual environment."
      exit 1
  fi
  echo "Virtual environment created successfully."
}

function activate_venv() {
  echo "Activating virtual environment..."
  source "${VENV_DIR}/bin/activate"
}

function install_dependencies() {
    echo "Installing dependencies..."
    pip install --upgrade pip
    pip install -e "${PROJECT_DIR}"
    if [[ $? -ne 0 ]]; then
        echo "Error: Could not install dependencies."
        exit 1
    fi
    echo "Dependencies installed successfully."
}

function run_tests(){
    echo "Running tests..."
    # add here your test command
    echo "tests finished"
}

function main(){
  # Check if the virtual environment exists
  if [[ ! -d "${VENV_DIR}" ]]; then
    create_venv
  fi

  activate_venv

  install_dependencies

  # Optionally run tests
  # run_tests
}

main