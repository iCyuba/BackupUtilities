#!/bin/zsh

# Run this to update X amount of the files in ./generated

# Pick random files
max=${1:-10}
files=$(find ./generated -type f | sort -R | head -n $max)

# Pick a random method, if none is given (possible methods: delete, regen, random)
method=${2:-"random"}
if [ "$method" = "random" ]; then
    method=$(echo "delete\nregen" | sort -R | head -n 1)
fi

# Run the method on each file
while IFS= read -r file; do
    case "$method" in
    delete)
        rm "$file"
        ;;
    regen)
        length=$(($RANDOM % 1000 + 1))
        cat /dev/urandom | base64 | head -c $length >"$file"
        ;;
    esac

    echo $file - $method
done <<<"$files"
