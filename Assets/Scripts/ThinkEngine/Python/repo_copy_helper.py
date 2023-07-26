import glob
import os
import sys
from tqdm import tqdm
import shutil

repo_path = "C:/Dev/ThinkEngine/it"
repo_in_project_path = "C:/Dev/CookedUp/Assets/ThinkEngineer/ThinkEngine/ThinkEngine/it"

pull = False
push = False


def handle_args():
    global pull
    global push
    i = 1
    while i < len(sys.argv):
        if sys.argv[i] in ['-h', '--help']:
            print(f"Usage: python repo_copy_helper.py [options]")
            print(f"Options:")
            print(f"  -h, --help: show this help")
            print(f"  pull: pull from repo")
            print(f"  push: push to repo")
            exit(0)
        elif sys.argv[i] in ['pull']:
            pull = True
        elif sys.argv[i] in ['push']:
            push = True
        i += 1
    
    if pull and push:
        print("Both pull and push given")
        exit(1)
    elif not pull and not push:
        print("No option given")
        exit(1)
            

def pull_from_repo():
    shutil.rmtree(repo_in_project_path)
    shutil.copytree(repo_path, repo_in_project_path)

def push_to_repo():
    shutil.rmtree(repo_path)
    shutil.copytree(repo_in_project_path, repo_path)


def main():
    handle_args()
    if pull:
        print("Pulling from repo")
        pull_from_repo()
    elif push:
        print("Pushing to repo")
        push_to_repo()
    else:
        print("No option given")
        exit(1)
    print("Done")


if __name__ == "__main__":
    main()
