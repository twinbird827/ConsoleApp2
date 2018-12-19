ConsoleApp2 - Arrangay
====

## Overview

このアプリはコンソールアプリです。
生成された実行ファイルにディレクトリをドロップすると処理が実行されます。

## Description

ディレクトリがドロップされると、ディレクトリ内のサブディレクトリにあるファイルが全て
ルートディレクトリに移動されます。

移動後にファイル名を5桁ゼロ埋めの連番でリネームします。
リネーム後は、ファイルは以下の順で並びます。
ルートディレクトリ内のファイル→colorディレクトリ内のファイル→その他ディレクトリのファイル

この処理は以下の拡張子を除外します。除外したファイルは削除されます。
".dll", ".htm", ".lnk", ".url", ".html"

この処理は以下のディレクトリを除外します。除外したディレクトリは削除されます。
"単ページ"

## Demo

## VS. 

## Requirement

.NET Framework 4.5.2

## Usage

1. C:\Testを対象にして実行します。
Arrangay.exe "C:\Test"

2. C:\Test1とC:\Test2を対象にして実行します。
Arrangay.exe "C:\Test1" "C:\Test2" ...

## Install

ビルドして生成した実行ファイルを任意のディレクトリに配置してください。

## Contribution

## Licence

[MIT](https://github.com/twinbird827/ConsoleApp2/blob/master/LICENSE)

## Author

[twinbird827](https://github.com/twinbird827)