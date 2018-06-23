import base64

with open('stage3.txt', 'rb') as input_file:
	coded_string = input_file.read()
	decoded = base64.b64decode(coded_string)
	with open('stage3.exe', 'wb') as output_file:
		output_file.write(decoded)