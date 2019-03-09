module.exports = {
	resolve: {
		extensions: ['.js', '.jsx']
	},
	entry: './src/index.js',
	output: {
		filename: 'bundle.js',
		path: __dirname + '/public'
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
				}
			}
		]
	},
	externals: {
		config: JSON.stringify({
			apiUrl: 'http://localhost:4000'
		})
	}
};