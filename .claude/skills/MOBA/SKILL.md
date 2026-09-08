```markdown
# MOBA Development Patterns

> Auto-generated skill from repository analysis

## Overview
This skill teaches you the core development patterns and conventions used in the MOBA TypeScript codebase. You'll learn how to structure files, write imports and exports, follow commit message guidelines, and organize tests. These patterns ensure consistency and maintainability across the project.

## Coding Conventions

### File Naming
- Use **camelCase** for file names.
  - Example: `playerManager.ts`, `gameLogic.ts`

### Import Style
- Use **relative imports** for referencing other modules.
  - Example:
    ```typescript
    import { Player } from './playerManager';
    ```

### Export Style
- Use **named exports** for all modules.
  - Example:
    ```typescript
    // In playerManager.ts
    export function createPlayer() { ... }
    export const MAX_PLAYERS = 10;
    ```

### Commit Messages
- Follow **Conventional Commits**.
- Prefixes: `feat` (features), `docs` (documentation).
- Keep commit messages concise (average 42 characters).
  - Example:
    ```
    feat: add player movement logic
    docs: update README with setup instructions
    ```

## Workflows

### Feature Development
**Trigger:** When adding a new feature  
**Command:** `/feature-dev`

1. Create a new TypeScript file using camelCase naming.
2. Implement the feature using named exports.
3. Use relative imports for dependencies.
4. Write or update corresponding test files (`*.test.ts`).
5. Commit changes with a `feat:` prefix and a concise message.

### Documentation Update
**Trigger:** When updating or adding documentation  
**Command:** `/docs-update`

1. Edit or create documentation files as needed.
2. Commit changes with a `docs:` prefix and a concise message.

## Testing Patterns

- Test files follow the pattern: `*.test.*` (e.g., `playerManager.test.ts`).
- The specific testing framework is not detected; follow the existing test file structure.
- Place test files alongside the modules they test or in a dedicated test directory.
- Example test file:
  ```typescript
  // playerManager.test.ts
  import { createPlayer } from './playerManager';

  describe('createPlayer', () => {
    it('should create a player with default values', () => {
      const player = createPlayer();
      expect(player).toBeDefined();
    });
  });
  ```

## Commands
| Command         | Purpose                                      |
|-----------------|----------------------------------------------|
| /feature-dev    | Start a new feature using project conventions |
| /docs-update    | Update or add documentation                  |
```
