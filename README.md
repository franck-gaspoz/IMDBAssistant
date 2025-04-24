# ![icon](./assets/multimedia-small.png) Movie Db Assistant

[![GPLv3 license](https://img.shields.io/badge/License-GPLv3-blue.svg)](https://raw.githubusercontent.com/franck-gaspoz/MovieDbAssistant/refs/heads/main/LICENSE)
![.net](https://img.shields.io/static/v1?label=&message=.NET%208&color=307639&style=plastic&logo=.net) 
![csharp](https://img.shields.io/static/v1?label=&message=C%20&sharp;&color=cdf998&style=plastic&logo=csharp&logoColor=dodgerblue)
![javascript](https://img.shields.io/static/v1?label=&message=javascript&color=cdf998&style=plastic&logo=javascript&logoColor=darkgreen)
![json](https://img.shields.io/static/v1?label=&message=JSON&color=cdf998&style=plastic&logo=javascript&logoColor=darkgreen)
![html5](https://img.shields.io/static/v1?label=&message=HTML5&color=cdf998&style=plastic&logo=html5) ![css3](https://img.shields.io/static/v1?label=&message=CSS3&color=cdf998&style=plastic&logo=css3&logoColor=black)
![linux](https://img.shields.io/static/v1?label=&message=Linux&color=285fdd&style=plastic&logo=linux) ![windows](https://img.shields.io/static/v1?label=&message=Windows&color=285fdd&style=plastic&logo=windows&logoColor=77DDFF) ![osx](https://img.shields.io/static/v1?label=&message=OSX&color=285fdd&style=plastic&logo=apple&logoColor=AAFFAA)

___

Generates **Movie catalogs documents** (HTML, stand alone ZIP,...) from a **list of movies titles** using **Web Crawlers** and a **templating system**.
👉 Can scrap data from [IMDb](http://www.imdb.com)

```mermaid
---
title: flow chart
---
    flowchart LR

    txt>list of movies titles .txt<br>----------------------------<br>title 1<br>title 2<br>...<br>title n<br>]
    mdba[Movie Db Assistant]
    scrawlers[[scrawlers]]
    websites[(web sites)]
    tple[[template engine]]
    tpls>templates:<br>--------------<br>front engine js<br>themes<br>styles css<br>media]
    mvc[(movie catalog web site)]
    qrc[(queries results cache)]
    dit[[data integrator]]
    oc[(output cache)]
    mdb[(movie db)]

    txt-- parse input file -->mdba

    mdba-- scrawlers queries -->qrc
    mdba-- movies data -->dit

    dit-- movies data -->tple
    dit-- store -->mdb 

    qrc-- json -->mdba
    qrc-- scrawlers queries -->scrawlers

    scrawlers-- scrawl -->websites 

    tpls-.->tple

    tple-- generates -->mvc 
    
    oc<-- files -->tple
```

👉 User & Developer manual: [manual.md](doc/manual.md)

<br>
<table width="100%" border="0">
<tr>
<td>
    <p align="center">
        <img src="https://raw.githubusercontent.com/franck-gaspoz/MovieDbAssistant/main/assets/snap-list.png" 
          width="70%" 
          align="center" style="margin-left:auto;margin-right:auto" 
          alt="html movie catalog in browser: movie list">
        <br><br><i>html movie catalog in browser: movie list</i>
    </p>
</td>
<td>
    <p align="center">
        <img src="https://raw.githubusercontent.com/franck-gaspoz/MovieDbAssistant/main/assets/snap-details.png" 
          width="70%" 
          align="center" style="margin-left:auto;margin-right:auto" 
          alt="html movie catalog in browser: movie details">
        <br><br><i>html movie catalog in browser: movie details</i>
    </p>
</td>
</tr>
</table>

## Install

👉 download last release: [1.0.0](https://github.com/franck-gaspoz/MovieDbAssistant/releases/download/v1.0.0/movie-db-assistant.setup.1.0.0.exe) *(Inno Setup)* for `Windows 7/8/10/11 x64`

- previous: [1.0.0-beta](https://github.com/franck-gaspoz/MovieDbAssistant/releases/download/1.0.0-beta/movie-db-assistant.setup.1.0.0.exe)

<p align="center">
<img src="https://raw.githubusercontent.com/franck-gaspoz/MovieDbAssistant/main/assets/setup-iss.png" 
  width="35%" 
  align="center" style="margin-left:auto;margin-right:auto" 
  alt="Inno Setup">
</p>

## Run

This application run as a tray icon

:arrow_right: run from the `Windows Start Menu`: Movie Db Assistant

<br>
<table width="100%" border="0" align="center">
<tr>
<td>
    <p align="center">
        <img src="https://raw.githubusercontent.com/franck-gaspoz/MovieDbAssistant/main/assets/tray-icon.png" 
          width="70%" 
          align="center" style="margin-left:auto;margin-right:auto" 
          alt="tray icon">
        <br><br><i>tray icon</i>
    </p>
</td>
<td>
    <p align="center">
        <img src="https://raw.githubusercontent.com/franck-gaspoz/MovieDbAssistant/main/assets/tray-menu.png" 
          width="70%" 
          align="center" 
          style="margin-left:auto;margin-right:auto"
          alt="tray menu">
        <br><br><i>tray menu</i>
    </p>
</td>
</tr>
</table>

## Build

 optional steps if you wish to build the app from source

### System Tray application for ![windows](https://img.shields.io/static/v1?label=&message=Windows&color=285fdd&style=plastic&logo=windows&logoColor=77DDFF)™ 10.0.22621.0 and above 

Available on `Windows 10.0.22621.0` and more

The project has currently no GUI for `OSX` and `Linux systems`, even if the app core is multi plateform.

#### Build & Run from source

- :arrow_right: build `MovieDbAssistant.app` and run 🗔 `MovieDbAssistant.exe`

- :arrow_right: select an action from the tray menu

:point_right: consult the **manual** here : [manual.md](/doc/manual.md)

#### Build an application appx package

You can build an appx app from the command line:
- :arrow_right: folder `/package` in repo root contains scripts (that must be adapted to your needs) that helps to build an `msix` application package:
    
    - `make.ps1`: prepare a package
    - `maje-cert.ps1`: create a certificate and sign a package

____

## Projects & Dependencies

- **App : Tray Icon GUI for Windows™**

    - `SDK`: `Microsoft.NET.Sdk` (`Microsoft.NETCore.App`)
    - framework `net8.0-windows10.0.22621.0` (`Microsoft.Windows.Desktop.App.WindowsForms`)
    - `OS`: Windows 10.0.22621.0 and +
    ![windows](https://img.shields.io/static/v1?label=&message=Windows&color=285fdd&style=plastic&logo=windows&logoColor=77DDFF)

- **App.Core** : application core (*🚧coming soon🚧*)

    - `SDK`: `Microsoft.NET.Sdk` (`Microsoft.NETCore.App`)
    - `OS`: ![linux](https://img.shields.io/static/v1?label=&message=Linux&color=285fdd&style=plastic&logo=linux) ![windows](https://img.shields.io/static/v1?label=&message=Windows&color=285fdd&style=plastic&logo=windows&logoColor=77DDFF) ![osx](https://img.shields.io/static/v1?label=&message=OSX&color=285fdd&style=plastic&logo=apple&logoColor=AAFFAA)

- **Dmn** : app domain

    - `SDK`: `Microsoft.NET.Sdk` (`Microsoft.NETCore.App`)
    - `MovieDbSpiders`: [/MovieDbScraper/blob/master/README.md](https://github.com/franck-gaspoz/MovieDbSpiders/blob/master/README.md)
    - `NewtonSoft.Json`: [https://github.com/JamesNK/Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)
    - `OS`: ![linux](https://img.shields.io/static/v1?label=&message=Linux&color=285fdd&style=plastic&logo=linux) ![windows](https://img.shields.io/static/v1?label=&message=Windows&color=285fdd&style=plastic&logo=windows&logoColor=77DDFF) ![osx](https://img.shields.io/static/v1?label=&message=OSX&color=285fdd&style=plastic&logo=apple&logoColor=AAFFAA)

- **Lib** : library (infrastructure)

    - `SDK`: `Microsoft.NET.Sdk` (`Microsoft.NETCore.App`)
    - `OS`: ![linux](https://img.shields.io/static/v1?label=&message=Linux&color=285fdd&style=plastic&logo=linux) ![windows](https://img.shields.io/static/v1?label=&message=Windows&color=285fdd&style=plastic&logo=windows&logoColor=77DDFF) ![osx](https://img.shields.io/static/v1?label=&message=OSX&color=285fdd&style=plastic&logo=apple&logoColor=AAFFAA)
___

## Credits

- <a target="_blank" href="https://www.flaticon.com/free-icons/movie-theater" title="movie theater icons">Movie theater icons created by Freepik - Flaticon</a>
- <a target="_blank" href="https://www.flaticon.com/free-icons/backward" title="backward icons">Backward icons created by Md Tanvirul Haque - Flaticon</a>
- <a target="_blank" href="https://www.flaticon.com/free-icons/house-agent" title="house-agent icons">House-agent icons created by Ihdizein - Flaticon</a>
- <a target="_blank" href="https://icons8.com/icon/11511/reply-arrow">Back</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
- <a target="_blank" href="https://icons8.com/icon/60449/play-button-circled">Play</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
- <a target="_blank" href="https://icons8.com/icon/23353/downloading-updates">Download</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
- <a target="_blank" href="https://icons8.com/icon/364/settings">Settings</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
- <a target="_blank" href="https://icons8.com/icon/23537/close-window">Close Window</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
- <a target="_blank" href="https://icons8.com/icon/C19x5dib8DcR/circular-arrows">Circular Arrows</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
- <a target="_blank" href="https://icons8.com/icon/SYOcualEVLca/spinner">Spinner</a> icon by <a target="_blank" href="https://icons8.com">Icons8</a>
___

## Releases History

2024/23/11 - 1.0.0

- media providers sources + icons
- presentation improvements
- bug fixes
- catalog test
- documentation
- media resources
- refactoring
- movie db software suite constituted

2024/04/11 - 1.0.0-beta - initial version

functionalities:

- build html movie catalogs documents from:
    - use [MovieDbSpiders](https://github.com/franck-gaspoz/MovieDbSpiders/blob/master/README.md) `json` output
    - direct scrap from queries given in files (several formats are supported)
    - multi format queries
    - direct scrap from a query given in last text clipboard entry
    - html catalog templates
    - html catalog resources : backgrounds, fonts, css, js template engine

- Windows setup for System Tray GUI
    - Inno Setup
    - Windows™.msix/.appx
