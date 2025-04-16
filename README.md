# Graphics Package

A comprehensive C# graphics package for rendering 2D and 3D scenes with advanced features and efficient performance.

## Overview

This WPF-based graphics package provides tools for creating, rendering, and manipulating graphical elements using various algorithms. It implements core computer graphics concepts from the ground up, providing both educational value and practical functionality.

## Features

### Line Drawing Algorithms
- **DDA (Digital Differential Analyzer)**: Line drawing using floating-point calculations for smooth results
- **Bresenham's Algorithm**: Efficient line drawing using only integer arithmetic

### User Interface
- Intuitive coordinate input system
- Color selection for drawing operations
- Clean, modern dark theme interface
- Real-time rendering on a dedicated canvas

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or later (recommended)

### Installation
1. Clone the repository:
   ```
   git clone https://github.com/your-username/Graphics-Package.git
   ```
2. Open `src/GraphicsPackage.sln` in Visual Studio
3. Build and run the project

## Usage

1. **Drawing Lines**:
   - Enter the starting coordinates (Point 1)
   - Enter the ending coordinates (Point 2)
   - Select a color from the dropdown
   - Click either "Draw Line (DDA)" or "Draw Line (Bresenham)" to draw using the respective algorithm

2. **Clearing the Canvas**:
   - Click the "Clear Canvas" button to reset the drawing area

## Technical Implementation

The Graphics Package uses the following key techniques:

- **WriteableBitmap** for direct pixel manipulation
- **Unsafe code blocks** for efficient memory access when setting pixels
- **Event-driven architecture** for interactive UI

## Future Enhancements

- Circle and ellipse drawing algorithms
- Polygon creation and filling
- Clipping algorithms
- 2D and 3D transformations (translation, rotation, scaling)
- 3D rendering capabilities

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.
