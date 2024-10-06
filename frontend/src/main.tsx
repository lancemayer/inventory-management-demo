import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import App from "./App";
import Home from "./app/Home";
import Item from "./app/Item";
import Items from "./app/Items";
import "./index.css";

const router = createBrowserRouter([
	{
		element: <App />,
		children: [
			{
				path: "/",
				element: <Home />,
			},
			{
				path: "/item",
				element: <Item />,
			},
			{
				path: "/items",
				element: <Items />,
				loader: async () => {
					return fetch(`${import.meta.env.VITE_API_URL}/items`);
				},
			},
		],
	},
]);

createRoot(document.getElementById("root")!).render(
	<StrictMode>
		<RouterProvider router={router} />
	</StrictMode>
);
