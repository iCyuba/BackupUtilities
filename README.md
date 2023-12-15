# ![Backup Utility](assets/full.svg)

## A school project

Written in C# using **.NET 8.0**

This utility recursively copies a set of directories to any number of destinations.
A cron job can be used to schedule the backup.

## Usage

Available arguments:

-   `--config`: Path to the configuration file. Defaults to `config.json` in the current working directory
-   `--once`: Run the backup once and exit. Defaults to false (to keep parity with the given task)
-   `--help` / `-h` / `-?`: Displays this help message
-   `--version`: Displays the version of the program

## Configuration

The configuration file is a JSON file with the following structure:

-   `sources`: List of directories to backup
-   `ignore`: List of regular expressions, matching files and directories to ignore (optional)
-   `targets`: List of directories to backup to
-   `method`: Backup method to use. Can be `full`, `differential` or `incremental`
-   `timing`: Cron expression to schedule the backup
-   `retention`: Retention policy
    -   `count`: Number of backups packages to keep
    -   `size`: Maximum amount of partial backups in a package
-   `output`: Output format. Available options: `folder`, `tar`, `tar.gz`, `tar.bz2` or `zip`. (optional, defaults to `folder`)

The configuration file is validated against [this json schema](/src/schemas/config.json).

### Examples

#### Bare minimum

```json
[
    {
        "sources": ["/path/to/source"],
        "targets": ["~/target"],
        "method": "incremental",
        "timing": "* * * * *",
        "retention": {
            "count": 5,
            "size": 2
        }
    }
]
```

#### With additional options

```json
[
    {
        "sources": ["/path/to/source", "./another/source"],
        "ignore": [".git", ".vscode", "node_modules"],
        "targets": ["~/target1", "/target2"],
        "method": "full",
        "timing": "* * * * *",
        "retention": {
            "count": 5,
            "size": 2
        },
        "output": "tar.gz"
    }
]
```
