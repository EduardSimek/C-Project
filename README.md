C# Application - comparing image processing libraries EmguCV and AForge.NET

This application is focused on the theoretical analysis of image processing algorithms and on the comparison of the processing time of the identical functions of the C# libraries - EmguCV and the AForge.NET. The main goal of the project was to measure a set of 10 processing times of the same image using the libraries EmguCV and AForge.NET and evaluation in the form of a graph.

The application consists of the next partial steps:
1. Downloading and implementation frameworks for image processing - AForge.NET and Emgu.CV in Visual Studio.
2. Implementation of some functions, which enable measuring time - this provides an external library Stopwatch and displaying charts - Zedgraph.
3. Search for common and different color models and filters.
4. Creating flow charts.
5. Performing 10 measurements for each library and gain maximum, minimum and average times, which will be displayed in individual labels.
6. Next step will be to compare those color models and filters, which have those two frameworks in common.
7. Final evaluation of the obtained work.
8. Based on the results of the work, it will be evaluated which library (EmguCV or AForge.NET) processed individual algorithms in a shorter time horizon.

Flow chart from the main conversion input RGB image to the various color models and filters. 

![image](https://github.com/EduardSimek/C-Project/assets/89217170/66b2a4f4-7f4d-4274-abc8-f4be2acf2ba5)


Flow chart, which displays conversion input RGB image to the various colorful filters with the components GroupBox and TrackBar.

![image](https://github.com/EduardSimek/C-Project/assets/89217170/ea6c5983-c5c5-41d1-9cbc-b1c8103be4ce)







My results from comparing various colorful models are here: 



![image](https://github.com/EduardSimek/C-Project/assets/89217170/f0f0e7be-0293-4058-bda5-51ee34a4244e)










![image](https://github.com/EduardSimek/C-Project/assets/89217170/6722adb9-d9b5-4c65-b491-12a59fdcea14)







![image](https://github.com/EduardSimek/C-project/assets/89217170/281d729a-a5a7-4642-91b8-c15182c2990b)






![image](https://github.com/EduardSimek/C-Project/assets/89217170/18695b1c-a84c-4a87-86e1-d351fb470050)





