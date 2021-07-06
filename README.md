# Prüfung Kaufmann Mende

Umgesetzt werden soll eine C#-(Konsolen)anwendung zur Kategorisierung von Bildern, die vom User übergeben werden.
Das Modelltraining erfolgt im Entwicklungsprozess und wird mit ausgeliefert, kann aber auch vom User erneut durchgeführt werden.

Für das Training wird die AutoML-Funktionalität in der ML.NET-Erweiterung genutzt. Diese wählt automatisch den besten Algorithmus für das Training aus. 

Insgesamt lässt sich das Programm in mehrere Bestandteile gliedern:
* Bezug des Datasets aus einer Datenbank mit notwendigen Metadaten
* Modelbuilder und Modelconsumption für Bilder
* Eingabe von lokal gespeicherten Bildern (eines oder ein Ordner mit Bildern), Ausgabe der Kategorisierung in der Konsole oder csv-Datei, ggf. Sortierung der Bilder in Unterordner

