#!/bin/bash

if [ -z "$1" ]
then
   echo "You need to provide a version";
   exit 1
fi

git pull
git checkout development
git pull
git checkout master
NUM_COMMITS=`git log --oneline master ^development | wc -l | bc`
git rebase development
git rebase -i HEAD~$NUM_COMMITS
echo $1 > VERSION
git add VERSION
git commit -m "chore: bump version to $1"
git tag v$1
git push
git push --tags