import {
    Link
} from "react-router-dom";

export default function PaymentFailed() {

    return (

        <div>

            <h1>
                Payment Failed
            </h1>

            <Link to="/payment">
                Try Again
            </Link>

        </div>
    );
}