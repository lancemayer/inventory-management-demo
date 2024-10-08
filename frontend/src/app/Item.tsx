import { useEffect, useRef } from "react";
import { Form, useLoaderData, useNavigation } from "react-router-dom";

function Item() {
	const item = useLoaderData();

	const transition = useNavigation();
	const isAdding = transition.state === "submitting";

	const formRef = useRef();

	useEffect(() => {
		if (!isAdding) {
			formRef.current?.reset();
		}
	}, [isAdding]);

	return (
		<>
			<div className="p-3 border-b-2">
				<p>Item Details</p>
			</div>
			<Form ref={formRef} method="post">
				<p>Quantity</p>
				<input className="border-2" name="quantity" type="number" />
				<p>Reason</p>
				<input className="border-2" name="reason" type="text" />
				<input name="id" type="hidden" value={item.id} />
				<div className="my-2">
					<button className="p-2 rounded-md bg-blue-600 text-white">
						Adjust Quantity
					</button>
				</div>
			</Form>
			<div>
				<p>{item.name}</p>
				<p>{item.description}</p>
				<p>{item.createdAt}</p>
				<p>{item.deletedAt}</p>
			</div>
			<table className="p-3 border-b-2">
				<thead>
					<tr>
						<th>Quantity</th>
						<th>Reason</th>
						<th>Time</th>
					</tr>
				</thead>
				<tbody>
					{item.events.map((event: any) => (
						<tr key={event.id}>
							<td>{event.quantity}</td>
							<td>{event.reason}</td>
							<td>{event.createdAt}</td>
						</tr>
					))}
				</tbody>
			</table>
		</>
	);
}

export default Item;
