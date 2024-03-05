#!/bin/zsh

# Generate random files in random subdirectories of ./generated
mkdir -p ./generated

random_files() {
    local dir="$1"

    local files=$(($RANDOM % $2 + 1))
    for i in $(seq 0 $files); do
        local name="$(cat /dev/urandom | base64 | head -c 10)"
        name=$(echo $name | sed 's/\//-/g')

        local length=$(($RANDOM % 1000 + 1))

        cat /dev/urandom | base64 | head -c $length >"$dir/$name.txt"
    done
}

random_dirs() {
    local dir="$1"

    local min=${3:-0}
    local count=$(($RANDOM % ($2 + 1) + $min))

    echo "count: $count - min: $min - max: $2"

    # Skip if count is 0 or less
    if [ $count -le 0 ]; then
        return
    fi

    for i in $(seq 0 $count); do
        local name="$(cat /dev/urandom | base64 | head -c 10)"
        mkdir -p "$dir/$name"

        # Make files in this directory
        random_files "$dir/$name" $2

        # Call again with less subdirectories
        random_dirs "$dir/$name" $(($2 - 1)) 0
    done
}

# Make between 5 and 10 subdirectories in ./generated (can be changed with arguments)
random_dirs ./generated ${1:-5} ${2:-5}
random_files ./generated ${1:-5}
