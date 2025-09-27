cd /d %~dp0

for %%f in (release debug beta plugin) do (
	docker compose run retouch pwsh work/EntryRetouch.ps1 -mode %%f
	docker compose run inkscape /bin/bash -ue /work/export-png.sh --svg /data/@data/%%f/App.svg --output /data/@data/%%f
	docker compose run --entrypoint "python /work/create-ico.py --dir /data/@data/%%f --output /data/@data/%%f/App.ico" python
)

