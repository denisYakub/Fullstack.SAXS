from flask import Flask, request, jsonify, Response
from flask_cors import CORS
import matplotlib.pyplot as plt
import mpld3
import json
import sys
import logging

log = logging.getLogger('werkzeug')
log.setLevel(logging.INFO)
log.handlers.clear()
log.addHandler(logging.StreamHandler(sys.stdout))

app = Flask(__name__)
CORS(app, resources={
    r"/plot": {
        "origins": [
            "http://localhost:5260",
            "https://localhost:7135"
        ]
    }
})

@app.route('/plot', methods=['POST'])
def plot():
    try:
        data = request.get_json(force=True)

        x = data.get("x", [])
        y = data.get("y", [])
        x_label = data.get("x_label", "X Axis")
        y_label = data.get("y_label", "Y Axis")
        title = data.get("title", "Graph")

        if not isinstance(x, list) or not isinstance(y, list):
            return jsonify({"error": "x and y must be lists"}), 400
        if len(x) != len(y):
            return jsonify({"error": "x and y must be the same length"}), 400
        if not all(isinstance(i, (int, float)) for i in x + y):
            return jsonify({"error": "x and y must contain only numbers"}), 400

        fig, ax = plt.subplots()
        ax.plot(x, y, marker='o')
        ax.set_xlabel(x_label)
        ax.set_ylabel(y_label)
        ax.set_title(title)

        html_str = mpld3.fig_to_html(fig)
        return Response(html_str, mimetype='text/html')

    except Exception as e:
        return jsonify({"error": str(e)}), 500


if __name__ == '__main__':
    app.run(debug=True, use_reloader=False, port=5001)
