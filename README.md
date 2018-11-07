# WekinatorExerciseClassifier

Classifying physical exercises based on accelerometer data taken from a phone via OSC.
After being classified, Unity receives the output data and plays an animation that corresponds to the exercise.

Structure:

Phone <-osc: phone data-> Unity <-osc: phone data-> Wekinator <-osc: classification output-> Unity

The data goes through Unity first for logging purposes, everything received is saved into a csv file when the data is relevant.
