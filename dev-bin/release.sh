#!/bin/bash

set -e

VERSION=$(perl -MFile::Slurp::Tiny=read_file -MDateTime <<EOF
use v5.16;
my \$today = DateTime->now->ymd;
my \$log = read_file(q{releasenotes.md});
\$log =~ /\n(\d+\.\d+\.\d+(?:-\w+)?) \((\d{4}-\d{2}-\d{2})\)\n/;
die "Release time is not today! Release: \$2 Today: \$today"
    unless \$today eq \$2;
say \$1;
EOF
)

TAG="v$VERSION"


if [ -n "$(git status --porcelain)" ]; then
    echo ". is not clean." >&2
    exit 1
fi

if [ ! -d .gh-pages ]; then
    echo "Checking out gh-pages in .gh-pages"
    git clone -b gh-pages git@github.com:maxmind/GeoIP2-dotnet.git .gh-pages
    pushd .gh-pages
else
    echo "Updating .gh-pages"
    pushd .gh-pages
    git pull
fi

if [ -n "$(git status --porcelain)" ]; then
    echo ".gh-pages is not clean" >&2
    exit 1
fi

popd

PAGE=.gh-pages/index.md
cat <<EOF > $PAGE
---
layout: default
title: MaxMind GeoIP2 .NET API
language: dotnet
version: $TAG
---

EOF

cat README.md >> $PAGE

xbuild /property:Configuration=Release
monodocer -assembly:GeoIP2/bin/Release/MaxMind.GeoIP2.dll -importslashdoc:GeoIP2/bin/Release/MaxMind.GeoIP2.XML -path:/tmp/dotnet-$TAG -pretty
mdoc export-html -o .gh-pages/doc/$TAG /tmp/dotnet-$TAG

pushd .gh-pages

git add doc/
git commit -m "Updated for $TAG" -a

read -e -p "Push to origin? " SHOULD_PUSH

if [ "$SHOULD_PUSH" != "y" ]; then
    echo "Aborting"
    exit 1
fi

git push

popd
git tag $TAG
git push
git push --tags
