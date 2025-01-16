import typer
from .lib import get_version_string

app = typer.Typer()

@app.command(help='Get the current version', name='get')
def get(prerelease: bool = False) -> None:
    version_string = get_version_string(prerelease)
    print(version_string, end='')

if __name__ == '__main__':
    app()