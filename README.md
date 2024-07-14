# SwapPuzzleSolver

## Link

## Overview

The application can compute the steps to solve the Swap puzzle.  
The functionality does not exist, the ability to manually solve and play puzzles.

- The top left button allows you to switch between menus.
    - BoardData:
        - Load, save, edit, and create new board.
        - Up to 20 boards can be saved.

    - PuzzleSolve:
        - Calculate the steps to solve the current board.
        - You can check the trace of the calculated steps.

- Rules of Swap puzzle:
    - When three or more consecutive colors are lined up on the same line, they disappear.
    - Panels with Empty bottom will fall.
    - The Swap operation can be performed if the panel can be made to disappear.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.


### Third-Party Licenses

This project uses the following third-party libraries:

- [UniTask](https://github.com/Cysharp/UniTask) - Licensed under the MIT License. See the UniTask [LICENSE](UniTask_LICENSE) file for details.