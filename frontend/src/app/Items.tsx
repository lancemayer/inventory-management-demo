import { Link, useLoaderData } from "react-router-dom";

function Items() {
	const items = useLoaderData();

	return (
		<>
			{items.map((item: any) => (
				<div key={item.id} className="p-3 border-b-2">
					<Link className="text-blue-700 underline" to={`/item/${item.id}`}>
						{item.name}
					</Link>
					<p>{item.description}</p>
					<p>Quantity: {item.quantity}</p>
				</div>
			))}
		</>
	);
}

export default Items;
