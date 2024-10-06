import { Outlet } from "react-router-dom";

function App() {
	return (
		<>
			<div className="p-3 border-b-2">
				<h1 className="text-5xl font-bold text-center">Inventory Management</h1>
				<a className="m-2 p-2 bg-blue-600 text-white rounded-md" href="/">
					Home
				</a>
				<a className="m-2 p-2 bg-blue-600 text-white rounded-md" href="/items">
					Items
				</a>
			</div>
			<Outlet />
		</>
	);
}

export default App;
