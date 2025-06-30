GITHUB_TOKEN?=${GITHUB_TOKEN}
GITHUB_USER?=${GITHUB_USER}

f format:
	@fd . -td --max-depth 1 --search-path . | xargs -P 8 -I _ sh -c "echo _; cd _; fd '\.cs$$' -tf -X dotnet csharpier {}; fd '\.cs$$' -tf -X dos2unix -q -r {};"
	@fd csproj$$ -X dos2unix -q -r {}

r restore:
	dotnet restore

c clean:
	dotnet clean
	rm -rf .artifacts

b build:
	dotnet build

p pack:
	dotnet pack

t test:
	dotnet test
	dotnet run --project XmlFormat.Tool -- --help
	dotnet run --project XmlFormat.Tool -- --version
	dotnet run --project XmlFormat.Tool -- --inline test.xml
	.artifacts/bin/XmlFormat.Tool/debug/XmlFormat.Tool --help
	.artifacts/bin/XmlFormat.Tool/debug/XmlFormat.Tool --version
	.artifacts/bin/XmlFormat.Tool/debug/XmlFormat.Tool --inline test.xml

publish:
	dotnet push
