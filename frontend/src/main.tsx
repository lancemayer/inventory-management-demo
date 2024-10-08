import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { createBrowserRouter, RouterProvider } from "react-router-dom";
import App from "./App";
import Item from "./app/Item";
import Items from "./app/Items";
import "./index.css";

const router = createBrowserRouter([
	{
		element: <App />,
		children: [
			{
				path: "/",
				element: <Items />,
				loader: async () => {
					return fetch(`${import.meta.env.VITE_API_URL}/items`);
				},
			},
			{
				path: "/item/:id",
				element: <Item />,
				loader: async ({ params }) => {
					return fetch(`${import.meta.env.VITE_API_URL}/item/${params.id}`);
				},
				action: async ({ request }) => {
					const formData = await request.formData();

					const res = await fetch(
						`${import.meta.env.VITE_API_URL}/create-event`,
						{
							method: "POST",
							body: JSON.stringify({
								aggregateId: formData.get("id"),
								quantity: formData.get("quantity"),
								reason: formData.get("reason"),
							}),
							headers: new Headers({ "content-type": "application/json" }),
						}
					);

					if (!res.ok) throw res;

					return { ok: true };
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
