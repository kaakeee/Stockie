# Migration Progress: local file IO to Azure Storage Mount

Important Guideline:
1. When you use terminal command tool, never input a long command with multiple lines, always use a single line command.
2. When performing semantic or intent-based searches, DO NOT search content from `.appmod/` folder.
3. Never create a new project in the solution, always use the existing project to add new files or update the existing files.
4. Minimize code changes: update only what's necessary for the migration.
5. Add New Package References to Projects following the project SDK style rules.

Migration tasks checklist:
- [X] Create `.appmod/.migration/plan.md` with migration plan
- [X] Create `.appmod/.migration/progress.md` with initial checklist and guidelines
- [ ] [ ] Check version control status and stash uncommitted changes if any
- [ ] Create migration branch `appmod/dotnet-migration-local-file-io-to-azure-storage-mount-[CurrentTimestamp]`
- [ ] Update `Stockie\Data\DatabaseHelper.cs` to use `AZURE_MOUNT_PATH` and `Path.Combine` for AttachDbFilename
- [ ] Build solution and fix compile errors
- [ ] Run CVE check for added packages (if any)
- [ ] Finalize changes and commit each task

Version control notes:
- Do not commit files under `.appmod/`.
- All other changes must be committed on the migration branch.

