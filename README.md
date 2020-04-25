# MReader

This reader is to be used mainly to display pictures (*e.g.* .png, .jpeg, .bmp) that are higher than wide. Its purpose is to ease the reading of book pages, manga pages or any pages that are meant to be read like a book. 

This is a WPF (Windows Presentation Foundation) project to help me learn .NET Core and practice C# desktop application development. 

## It implements

* MVVM pattern ([Prism](https://prismlibrary.com/docs/))
* Service pattern
* Dependency injection ([Prism](https://prismlibrary.com/docs/) with [Unity container](https://github.com/unitycontainer/unity))

## How to use it

The interface is very basic at this stage. 

### Buttons

* **Open** : Open a new file
* **Lock** : Lock or unlock the splitters
* **Mode** : Switch from single panel to resizable panel mode
* **FullScreen** : Enables full screen
* **Add Message** : Add an error message to the message box 

### Shortcuts

* **<kbd>Ctrl</kbd> + <kbd>O</kbd>** : Open a new file
* **<kbd>Ctrl</kbd> + <kbd>ENTER</kbd>** : Enable/disable full screen
* **<kbd>Ctrl</kbd> + <kbd>B</kbd>** : Hide or show the toolbars
* **<kbd>Ctrl</kbd> + <kbd>L</kbd>** : Hide or show the message window
* **<kbd>LEFT</kbd>** : Load previous picture in folder
* **<kbd>RIGHT</kbd>** : Load next picture in folder


## Coming features
* Scrolling with arrow keys
* Loading of next and previous pictures by clicking
* Zoom
* Customization panel
* Themes
