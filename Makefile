CONFIGURATION ?= Release
CONSOLE_FRAMEWORK ?= netcoreapp2.0
DOTNETCORE ?= 1
MAXMIND_BENCHMARK_DB = /project/MaxMind.GeoIP2.Benchmark/GeoLite2-City.mmdb

define update_project
	@docker run \
		-it \
		--rm \
		-v $$(pwd)/$(1):/project \
		geoip2-dotnet \
		dotnet outdated -u
endef

benchmark:
	@docker run \
		-it \
		--rm \
		-v $$(pwd)/$(1):/project \
		-e "DOTNETCORE=$(DOTNETCORE)" \
		-e "MAXMIND_BENCHMARK_DB=$(MAXMIND_BENCHMARK_DB)" \
		geoip2-dotnet \
		/bin/sh -c "dotnet run \
			-c $(CONFIGURATION) \
			-f $(CONSOLE_FRAMEWORK) \
			-p ./MaxMind.GeoIP2.Benchmark/MaxMind.GeoIP2.Benchmark.csproj"

build-image:
	docker build -t geoip2-dotnet .

init: build-image
	git submodule update --init --recursive

test:
	@docker run \
		-it \
		--rm \
		-v $$(pwd)/$(1):/project \
		-e "DOTNETCORE=$(DOTNETCORE)" \
		geoip2-dotnet \
		/bin/sh -c "dotnet restore ./MaxMind.GeoIP2.sln && dotnet test \
			-c $(CONFIGURATION) \
			-f $(CONSOLE_FRAMEWORK) \
			./MaxMind.GeoIP2.sln"

update_dependencies:
	$(call update_project,MaxMind.GeoIP2)
	$(call update_project,MaxMind.GeoIP2.Benchmark)
	$(call update_project,MaxMind.GeoIP2.UnitTests)
