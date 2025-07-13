from flask import Flask, request, Response
import matplotlib.pyplot as plt
import mpld3
import json

app = Flask(__name__)

@app.route('/plot', methods=['POST'])
def plot():
    data = request.get_json()
    x = data.get("x", [])
    y = data.get("y", [])

    fig, ax = plt.subplots()
    ax.plot(x, y, marker='o')
    ax.set_title("Your Graph")

    html_str = mpld3.fig_to_html(fig)
    return Response(html_str, mimetype='text/html')

if __name__ == '__main__':
    app.run(debug=True, use_reloader=False, port=5001)