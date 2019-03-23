var path = require('path');
var HTMLWebpackPlugin = require('html-webpack-plugin');

module.exports = {
	resolve: {
		extensions: ['.js', '.jsx']
	},
	entry: './src/index.js',
	output: {
		filename: 'bundle.js',
		path: path.resolve(__dirname, 'dist'),
	},
	devServer: {
		historyApiFallback: true
	},
	module: {
		rules: [
			{
				test: /\.jsx?$/,
				exclude: /node_modules/,
				use:{
					loader: "babel-loader"
				},
				options: {
					presets: ['@babel/preset-env']
				  }
			}
		]
	},
	plugins: [new HTMLWebpackPlugin()]
	,
	externals: {
		config: JSON.stringify({
			apiUrl: 'http://localhost:4000'
		})
	}
};