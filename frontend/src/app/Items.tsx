import { useLoaderData } from "react-router-dom";

function Items() {
	const items = useLoaderData();

	return (
		<>
			{items.map((item: any) => (
				<div key={item.id} className="p-3 border-b-2">
					<p>{item.name}</p>
					<p>{item.description}</p>
				</div>
			))}
		</>
	);
}

export default Items;
