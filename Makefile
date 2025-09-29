SOURCE_DIRECTORY := $(dir $(realpath $(lastword $(MAKEFILE_LIST))))
ARTIFACT_PATH := $(SOURCE_DIRECTORY)artifacts
DOCS_PATH := $(SOURCE_DIRECTORY)docs
CONFIGURATION ?= Release

clean:
	dotnet clean
	rm -rf $(ARTIFACT_PATH)/*
	rm -rf $(DOCS_PATH)/api

restore:
	dotnet tool restore
	dotnet restore

build: restore
	dotnet build --configuration $(CONFIGURATION)

test: build
	dotnet test \
		--no-build \
		--configuration $(CONFIGURATION) \
		--filter '(Execution!=Manual)' \
		--blame \
		--diag "$(ARTIFACT_PATH)/diag.txt" \
		--logger "trx" \
		--collect "Code Coverage;Format=cobertura" \
		--results-directory $(ARTIFACT_PATH)/test-results \
		-- \
		RunConfiguration.CollectSourceInformation=true

generate-docs: clean restore
	cp $(SOURCE_DIRECTORY)src/Mscc.GenerativeAI/CHANGELOG.md $(SOURCE_DIRECTORY)docs/changelog.md
	cp $(SOURCE_DIRECTORY)src/Mscc.GenerativeAI/RELEASE_NOTES.md $(SOURCE_DIRECTORY)docs/release_notes.md
	dotnet docfx build $(DOCS_PATH)/docfx.json -- -p:Configuration=$(CONFIGURATION)

serve-docs: generate-docs
	dotnet docfx serve $(ARTIFACT_PATH)/_site --port 8080

.DEFAULT_GOAL := generate-docs