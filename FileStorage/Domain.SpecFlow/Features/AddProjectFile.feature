﻿Функционал: Добавление файла в проект
	При добавлении файла в проект, в папку проекта должно сохраняться содержимое файла. 
	В качестве имени файла используется имя проекта и порядковый номер файла внутри проекта.

Сценарий: Сохранять соержимое файла в папке проекта.
	Допустим в системе есть проект Prj1 и с папкой FileStorage\Prj1
	И есть следующие файлы
		| Id | Content         |
		| 1  | Hello my friend |
		| 2  | How do you do?  |
		| 3  | I am fine!      |
		| 4  | See you later   |
	Когда я в проект добавляю эти файлы
	Тогда в папке проекте должны появиться эти файлы
	И их содержимое должно соответсвовать