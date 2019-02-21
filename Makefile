define update_project
	docker run \
		-it \
		--rm \
		-v $$(pwd)/$(1):/project \
		geoip2-dotnet \
		dotnet outdated -u
endef

build-image:
	docker build -t geoip2-dotnet .

update_dependencies: build-image
	$(call update_project,MaxMind.GeoIP2)
	$(call update_project,MaxMind.GeoIP2.Benchmark)
	$(call update_project,MaxMind.GeoIP2.UnitTests)
