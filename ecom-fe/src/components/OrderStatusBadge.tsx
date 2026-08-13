type Props = {

    status: string;
};

export default function OrderStatusBadge({

    status

}: Props) {

    return (

        <span
            className={
                `status ${status}`
            }
        >

            {status}

        </span>
    );
}