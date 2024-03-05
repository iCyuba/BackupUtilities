#!/bin/zsh

# Clean up previous generated files
rm -rf ./generated
rm -rf ./backup

# Run a clean generation
./generate.sh ${1:-5} ${2:-5}
