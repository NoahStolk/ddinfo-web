# Generating database migration scripts

1. Create initial migration: `add-migration V1`
2. Make changes to entities.
3. Create new migration: `add-migration V2`
4. Generate script: `script-migration V1 V2`
